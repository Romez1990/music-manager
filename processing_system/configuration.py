import configparser
import os
from os import path


class Config:
    _config = None
    
    def __init__(self):
        if not Config._config:
            self.config_dir_path = path.expandvars(r'%APPDATA%\Music Manager')
            self.config_path = path.join(self.config_dir_path, 'config.cfg')
            
            Config._config = configparser.ConfigParser()
            if path.exists(self.config_path):
                Config._config.read(self.config_path)
            else:
                self.set_configuration_default()
    
    def set_configuration_default(self):
        from processing_system.configuration_default import configuration_default
        for section in configuration_default.keys():
            for option in configuration_default[section].keys():
                value = configuration_default[section][option]
                self.set_option_default(section, option, str(value))
        self.save()
    
    def get(self, section, option):
        return Config._config.get(section, option)
    
    def getboolean(self, section, option):
        return Config._config.getboolean(section, option)
    
    def set_option(self, section, option, value):
        if not Config._config.has_section(section):
            Config._config[section] = {}
        Config._config[section][option] = str(value)
    
    def set_option_default(self, section, option, value):
        if not Config._config.has_option(section, option):
            self.set_option(section, option, value)
    
    def save(self):
        if not path.exists(self.config_dir_path):
            os.makedirs(self.config_dir_path)
        file = open(self.config_path, 'w')
        Config._config.write(file)
