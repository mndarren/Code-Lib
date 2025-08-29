export PYTHONPATH = ./
.DEFAULT_GOAL := build
.PHONY: build publish publish-flightcheck docs venv


build:
	pip install --editable .

freeze:
	pip freeze | grep -v "pkg-resources*" > requirements.txt

lint:
	pylint stearns_boomerang

coverage: test
	mkdir -p docs/build/html
	coverage html

docs: coverage
	mkdir -p docs/source/_static
	mkdir -p docs/source/_templates
	cd docs && $(MAKE) html

publish-flightcheck: freeze
	python3 setup.py sdist

publish: test
	python3 setup.py sdist upload -r nexus

clean:
	rm -rf dist \
	rm -rf docs/build \
	rm -rf *.egg-info
	coverage erase

test: lint
	py.test --cov . tests/

venv:
	virtualenv --python python3.6 venv

install:
	pip install -r requirements.txt

