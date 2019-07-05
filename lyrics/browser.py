from selenium import webdriver
from configuration import Config


class Browser:
    def __init__(self):
        config = Config()
        self.browser = webdriver.Chrome(config.get('browser', 'driver'))
        if config.getboolean('browser', 'minimize'):
            self.browser.minimize_window()
    
    def __del__(self):
        self.browser.quit()
    
    def get_lyrics(self, url=None):
        if url is not None:
            self.browser.get(url)
        try:
            lyrics_element = self.browser.find_element_by_css_selector('.lyrics section p')
            lyrics = lyrics_element.text
            return lyrics
        except Exception:
            return None
    
    def open(self, url):
        self.browser.get(url)
    
    def maximize(self):
        self.browser.maximize_window()
    
    @staticmethod
    def exists(browser):
        if browser is None:
            browser = Browser()
        return browser
