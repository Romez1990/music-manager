import configparser
from os import path

path_to_config = 'config.cfg'
option_defaults = {
    'browser': {
        'driver':   r'C:\Program Files (x86)\Chrome Driver\Chrome Driver.exe',
        'minimize': True
    },
    'genius':  {
        'api_token': ''
    }
}


class Config:
    _config = None
    
    def __init__(self):
        if not Config._config:
            Config._config = configparser.ConfigParser()
            if path.exists(path_to_config):
                Config._config.read(path_to_config)
            else:
                for section in option_defaults.keys():
                    for option in option_defaults[section].keys():
                        value = option_defaults[section][option]
                        self.set_option_default(section, option, str(value))
                self.save()
    
    def set_option(self, section, option, value):
        if not Config._config.has_section(section):
            Config._config[section] = {}
        Config._config[section][option] = str(value)
    
    def set_option_default(self, section, option, value):
        if not Config._config.has_option(section, option):
            self.set_option(section, option, value)
    
    def save(self):
        with open(path_to_config, 'w') as file:
            Config._config.write(file)
    
    def get(self, section, option):
        return Config._config.get(section, option)
    
    def getboolean(self, section, option):
        return Config._config.getboolean(section, option)
