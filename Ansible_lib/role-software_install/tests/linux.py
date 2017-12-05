from testinfra.utils import ansible_roles
import pytest
import json


node_os = ansible_roles.get_node_os()

@pytest.mark.skipif(node_os['family'] != 'RedHat' and node_os['family'] != 'Suse', reason='RedHat/Suse only test')
class TestRole_sf_install:

    # default vars
    linux_tmp = '/var/tmp'
    sf_version = '6.2'
    linux_defaults = """
    {
      "linux_install": {
	"6.2": {
	  "install_package": "VRTS_SF_Basic_6.2_linux.tar.gz",
	  "pkg_deps": {
	    "RedHat6": [
	      "libstdc++",
	      "pam",
	      "libudev"
	    ],
	    "RedHat7": [],
	    "Suse11": []
	  },
	  "response_file": "sf_response_6.2.j2",
	  "sf_installer": {
	    "RedHat6": "/var/tmp/dvd3-sfbasic/rhel6_x86_64/installer",
	    "RedHat7": "/var/tmp/dvd3-sfbasic/rhel7_x86_64/installer",
	    "Suse11": "/var/tmp/dvd3-sfbasic/sles11_x86_64/installer"
	  }
	},
	"7.3": {
	  "install_package": "Veritas_InfoScale_7.3_RHEL.tar.gz",
	  "pkg_deps": {
            "RedHat7": [],
            "RedHat6": [],
            "Suse11": []
          },
	  "response_file": "sf_response_7.3.j2",
	  "sf_installer": {
	    "RedHat6": "/var/tmp//dvd1-redhatlinux/rhel6_x86_64/installer",
	    "RedHat7": "/var/tmp/dvd1-redhatlinux/rhel7_x86_64/installer",
	    "Suse11": "/var/tmp/dvd1-redhatlinux/sles11_x86_64/installer"
	  }
	},
	"linux_sf_url": "{{ artifactory_url }}/{{ artifactory_path }}"
      }
    }"""

    for call in ansible_roles.get_role_vars('sf_install'):
        osVersion = node_os['family'] + node_os['distribution_major_version']

        linux_install_defaults = json.loads(linux_defaults)['linux_install']

        linux_tmp = call.get('linux_tmp', linux_tmp)
        sf_version = call.get('sf_version', sf_version)
        linux_install_opts = call.get('linux_install', linux_install_defaults)[sf_version]

    @pytest.mark.parametrize("pkg", linux_install_opts['pkg_deps'][osVersion])
    def test_pkgs(self, host, pkg):
        pkg = host.package(pkg)
        assert pkg.is_installed

    def test_vxdisk(self, host):
        assert host.file('/opt/VRTS/bin/vxdisk').exists

    def test_response_file(self, host):
        assert host.file('%s/response_file' % self.linux_tmp)
