from os import getenv
from selenium import webdriver


class Browser:
    def __init__(self):
        self.browser = webdriver.Chrome(getenv('DRIVER'))
        if getenv('MINIMIZE_BROWSER').lower() in ('yes', 'true', '1'):
            self.browser.minimize_window()
    
    def __del__(self):
        self.browser.quit()
    
    def get_lyrics(self, url):
        self.browser.get(url)
        lyrics_element = self.browser.find_element_by_css_selector('.lyrics section p')
        lyrics = lyrics_element.text
        return lyrics
    
    @staticmethod
    def exists(browser):
        if browser is None:
            browser = Browser()
        return browser
