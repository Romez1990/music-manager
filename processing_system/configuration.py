import configparser
from sys import platform
from os import path, getenv, makedirs


class Config:
    class __Instance:
        def __init__(self):
            if platform == 'win32':
                self.config_dir_path = path.join(getenv('APPDATA'), 'Music Manager')
            else:
                self.config_dir_path = path.join(getenv('HOME'), '.config', 'music-manager')
            self.config_path = path.join(self.config_dir_path, 'config.cfg')
            
            self.config = configparser.ConfigParser()
            if path.exists(self.config_path):
                self.config.read(self.config_path)
            else:
                self.set_configuration_default()
        
        def set_option(self, section, option, value):
            if not self.config.has_section(section):
                self.config[section] = {}
            self.config[section][option] = str(value)
        
        def set_option_default(self, section, option, value):
            if not self.config.has_option(section, option):
                self.set_option(section, option, value)
        
        def set_configuration_default(self):
            from processing_system.configuration_default import configuration_default
            for section in configuration_default.keys():
                for option in configuration_default[section].keys():
                    value = configuration_default[section][option]
                    self.set_option_default(section, option, str(value))
            self.save()
        
        def save(self):
            if not path.exists(self.config_dir_path):
                makedirs(self.config_dir_path)
            file = open(self.config_path, 'w')
            self.config.write(file)
    
    __instance = None
    
    def __init__(self):
        if not Config.__instance:
            Config.__instance = Config.__Instance()
    
    def get(self, section, option):
        return Config.__instance.config.get(section, option)
    
    def getboolean(self, section, option):
        return Config.__instance.config.getboolean(section, option)
    
    def set_option(self, section, option, value):
        Config.__instance.set_option(section, option, value)
    
    def save(self):
        Config.__instance.save()
