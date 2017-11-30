#!/usr/bin/python
'''
This is a tool for run multiple playbooks, Command like:

python run_playbooks.py --password xxx --target_host xxx --hostname xxx 
       --bitbucket_user xxx --playbooks playbook1,playbook2,playbook1

Note: playbooks need to be separated by ','
'''
import os, sys
import re
import optparse
import getpass
from collections import namedtuple
from ansible.parsing.dataloader import DataLoader
from ansible.vars import VariableManager
from ansible.inventory import Inventory
from ansible.playbook import Playbook
from ansible.executor.playbook_executor import PlaybookExecutor

def get_args():
   parser = optparse.OptionParser("usage: %prog [--password] [--target_host]" +
                    "[--hostname] [--bitbucket_user] [--playbooks]")
   parser.add_option("--password", dest="password", type="string", help="password for root")
   parser.add_option("--target_host", dest="target_host", type="string", help="target machine ip address")
   parser.add_option("--hostname", dest="hostname", type="string", help="hostname for mint")
   parser.add_option("--bitbucket_user", dest="bitbucket_user", type="string", help="bitbucket user")
   parser.add_option("--playbooks", dest="playbooks", type="string", help="playbooks to be pulled")
   return parser.parse_args()

def process_playbooks(playbooks_to_run, bitbucket_user, target_ip, hostname, ansible_ssh_pass):
   playbooks = set(playbooks_to_run)
   bitbucket = "https://" + bitbucket_user + "@stash.veritas.com/scm/iac/"
   ansible_user = "root"
   directory = "/home/" + getpass.getuser() + '/runPlaybooks' + str(os.getpid())
   inventory = "jenkins.inv"

   run_command = "ansible-playbook -i " + inventory + " local.yml"
   pull_command = "ansible-galaxy install -r requirements.yml"
   clone_command = "git clone "

   pattern_mint = "playbook-mint"
   prog = re.compile(pattern_mint)

   create a folder to run multiple playbooks
   try:
      os.system("mkdir " + directory)
      os.chdir(directory)
   except Exception as e:
      raise Exception("Failed to create diretory: %s" % str(e))
      sys.exit()

   # pull playbooks
   try:
      for play in playbooks:
         clone_playbook = bitbucket + play + ".git"
         os.system(clone_command + clone_playbook)
   except Exception as e:
      raise Exception("Failed to clone playbooks: %s" % str(e))
      sys.exit()

   # pull roles for playbooks
   try:
      for play in playbooks:
        os.chdir(play)
        print("in dir: " + play)
        os.system(pull_command)
        os.chdir(directory)
   except Exception as e:
      raise Exception("Failed to pull roles: %s" % str(e))
      sys.exit()
   # invertory and run playbooks
   Options = namedtuple('Options', ['connection',  'forks', 'become', 'become_method',
    'become_user', 'check', 'listhosts', 'listtasks', 'listtags', 'syntax', 'module_path'])

   variable_manager = VariableManager()
   loader = DataLoader()
   options = Options(connection='local', forks=100, become=None, become_method=None, become_user=None,
       check=False, listhosts=False, listtasks=False, listtags=False, syntax=False, module_path="")
   passwords = ansible_ssh_pass #dict(vault_pass='secret')

   inventory = Inventory(loader=loader, variable_manager=variable_manager, host_list=target_ip)#'localhost')
   variable_manager.set_inventory(inventory)
   playbooks = playbooks_to_run #["./test.yaml"]

   executor = PlaybookExecutor(
              playbooks=playbooks,
              inventory=inventory,
              variable_manager=variable_manager,
              loader=loader,
              options=options,
              passwords=passwords)

   executor.run()
#   # create inventory file
#   try:
#      for play in playbooks:
#        os.chdir(play)
#        file = open(inventory, "w+")
#        file.write(target_ip + " ansible_user=" + ansible_user + " ansible_ssh_pass=" + ansible_ssh_pass)
#        if prog.search(play):
#          file.write(" hostname=" + hostname)
#        os.chdir(directory)
#        file.close()
#   except Exception as e:
#      raise Exception("Failed to create inventory file: %s" % str(e))
#      sys.exit()
#
#   # run playbooks
#   try:
#      for play in playbooks_to_run:
#         os.chdir(play)
#         os.system(run_command)
#         os.chdir(directory)
#   except Exception as e:
#      raise Exception("Failed to run playbooks: %s" % str(e))
#      sys.exit()

def main():
   options, args = get_args()
   playbooks_to_run = re.split(',', options.playbooks)
   bitbucket_user = options.bitbucket_user
   target_ip = options.target_host
   hostname = options.hostname
   ansible_ssh_pass = options.password
   process_playbooks(playbooks_to_run, bitbucket_user, target_ip, hostname, ansible_ssh_pass)

if __name__ == '__main__':
   main()
