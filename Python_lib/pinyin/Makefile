.DEFAULT_GOAL := build
.PHONY: build publish package package-test docs venv


build:
	pip install --editable .

freeze:
	pip freeze > requirements.txt

lint:
	pylint {{cookiecutter.project_slug}}

coverage: test
	mkdir -p docs/build/html
	coverage html

docs: coverage
	cd docs && $(MAKE) html

package: clean docs
	python setup.py sdist

package-test:
	python setup.py sdist

publish: package
	twine upload dist/* --repository nexus

clean :
	rm -rf dist \
	rm -rf docs/build \
	rm -rf *.egg-info
	coverage erase

test: lint
	py.test --cov . tests/

venv :
	virtualenv --python python3.6 venv

install:
	pip install -r requirements.txt