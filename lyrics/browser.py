from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException
from processing_system.configuration import Config


class Browser:
    def __init__(self):
        config = Config()
        self.browser = webdriver.Chrome(config.get('browser', 'driver'))
        if config.getboolean('browser', 'minimize'):
            self.minimize()
        else:
            self.maximize()
    
    def __del__(self):
        self.browser.quit()
    
    def get_lyrics(self, url=None):
        if url is not None:
            self.browser.get(url)
        try:
            lyrics_element = self.browser.find_element_by_css_selector('.lyrics section p')
        except NoSuchElementException:
            return None
        lyrics = lyrics_element.text
        return lyrics
    
    def open(self, url):
        self.browser.get(url)
    
    def minimize(self):
        self.browser.minimize_window()
    
    def maximize(self):
        self.browser.maximize_window()
    
    def focus(self):
        self.browser.switch_to.window(self.browser.current_window_handle)
