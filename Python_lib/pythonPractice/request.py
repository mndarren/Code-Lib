import requests

def gen_from_urls(urls: str) -> tuple:
	for resp in (requests.get(url) for url in urls):
		yield len(resp.content), resp.status_code, resp.url

urls = ('https://www.stcloudstate.edu', 'http://google.com', 'http://darrenhome.ddns.net')

for resp_len, resp_status, url in gen_from_urls(urls):
	print(resp_len, resp_status, url)
