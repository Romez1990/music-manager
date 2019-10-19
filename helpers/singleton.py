from typing import Callable, Dict, Any


def singleton(class_: type) -> Callable:
    instances: Dict[type, Callable] = {}

    def get_instance(*args: Any, **kwargs: Any) -> Callable:
        if class_ not in instances:
            instances[class_] = class_(*args, **kwargs)
        return instances[class_]

    return get_instance
