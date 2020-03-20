def query_yes_no(question: str, default=None) -> bool:
    valid = {'yes': True, 'y': True,
             'no': False, 'n': False}
    if default is None:
        prompt = ' [y/n] '
    elif default == 'yes':
        prompt = ' [Y/n] '
    elif default == 'no':
        prompt = ' [y/N] '
    else:
        raise ValueError(f'Invalid default answer: {default}')

    while True:
        choice = input(question + prompt).lower()
        if choice == '' and default is not None:
            return valid[default]
        elif choice in valid:
            return valid[choice]
        else:
            print('Please respond with yes or no')
