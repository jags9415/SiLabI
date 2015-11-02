import distutils.spawn
import subprocess
import getopt
import sys
import os


def exists_in_path(executable):
	return distutils.spawn.find_executable(executable) is not None

def execute_file(file, database="SiLabI"):
	if (os.path.isfile(file)):
		print '*', file
		subprocess.call(["sqlcmd", "-E", "-d", database, "-i", file], shell=True)
	else:
		print "ERROR: Archivo", file, "no encontrado."
		sys.exit(2)

def execute_directory(dir):
    for root, dirs, files in os.walk(dir):
        for file in files:
            if file.endswith(".sql"):
                path = os.path.join(root, file)
                execute_file(path)

def create():
	execute_file("create_database.sql", database="master")
	
	for dir in ["tables", "types", "functions", "views", "stored_procedures"]:
		execute_directory(dir)

	execute_file("create_users.sql")

def create_debug():
	create()
	execute_directory("fill_scripts")

def create_release():
	create()
	execute_file("fill_scripts/01-States.sql")
	execute_file("fill_scripts/02-PeriodTypes.sql")
	execute_file("fill_scripts/03-Periods.sql")
	execute_file("fill_scripts/11-Laboratories.sql")

def main(argv):
	configuration = "debug"

	if not exists_in_path("sqlcmd"):
		print "Ejecutable SQLCMD.exe no encontrado. Verifique que se encuentre en el PATH."
		sys.exit(2)

	try:
		opts, args = getopt.getopt(argv, "h", ["help", "release", "debug"])
	except getopt.GetoptError:
		print "usage: create.py [--release|--debug]"
		sys.exit(2)
	for opt, arg in opts:
		if opt in ("-h", "--help"):
			print "usage: create.py [--release|--debug]"
			sys.exit()
		elif opt == "--release":
			configuration = "release"
		elif opt == "--debug":
			configuration = "debug"

	if (configuration == "release"):
		create_release()
	elif (configuration == "debug"):
		create_debug()

if __name__ == "__main__":
	main(sys.argv[1:])