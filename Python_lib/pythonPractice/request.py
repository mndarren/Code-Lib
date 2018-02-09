import requests, pprint, pdb

#generator function
def gen_from_urls(urls: str) -> tuple:
	for resp in (requests.get(url) for url in urls):
		pdb.set_trace()
		yield len(resp.content), resp.status_code, resp.url

urls = ('https://www.stcloudstate.edu', 'http://google.com', 'http://darrenhome.ddns.net')

urls_resp = {url: size for size, _, url in gen_from_urls(urls)} #_ will ignore status
pprint.pprint(urls_resp)
