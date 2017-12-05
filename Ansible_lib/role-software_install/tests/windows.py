from testinfra.utils import conversion
from testinfra.utils import spec_range
from testinfra.utils import ansible_roles
import pytest

SF_FILE = "C:\\storagefoundation\\Veritas Volume Manager\\vxdisk.exe"
OUTPUT_CMD = "Harddisk0"

node_os = ansible_roles.get_node_os()

@pytest.mark.skipif(node_os['family'] != 'Windows', reason='Windows only test')
class TestSFInstalled:

   def test_vxdisk_exist(self, WinFile):
      corefile = WinFile(SF_FILE)
      assert corefile.exists
      assert corefile.is_file

   def test_git_lfs_url(self, Command):
      vxdisk_cmd_stdout = Command('vxdisk list').stdout
      assert OUTPUT_CMD in vxdisk_cmd_stdout
