function toggleTokenContainer(container) {
        if (switchControl.checked) {
        container.classList.add("token-container--visible");
    } else {
        container.classList.remove("token-container--visible");
    }
}

function tabActivation(e, panels) {
    e.preventDefault();

    for (var i = 0; i < panels.length; i++) {
        if (i == e.detail.index) {
            panels.item(i).classList.add('tab-content--active');
        } else {
            panels.item(i).classList.remove('tab-content--active');
        }
    }
}

function responseSuccess(token, button, tokenTxt, switchCtl) {
    button.attributes.removeNamedItem('disabled');
    tokenTxt.value = token;
    switchCtl.disabled = false;
}