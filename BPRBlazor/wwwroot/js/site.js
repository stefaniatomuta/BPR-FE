const fileName = "Download";

function DownloadResultsToPDF() {
    const elements = document.getElementsByClassName('pdf-image');
    const promise = Promise.resolve();

    for (let i = 0; i < elements.length; i++) {
        htmlToImage.toPng(elements[i].firstElementChild)
            .then(function (dataUrl) {
                const img = new Image();
                img.src = dataUrl;
                elements[i].firstElementChild.remove()
                elements[i].appendChild(img)
            }).catch()
    }
    window.focus()
    window.print()
}

function getElementOffset(elementId) {
    const element = document.getElementById(elementId);
    return { x: element.offsetLeft, y: element.offsetTop }
}

function getElementSize(elementId) {
    const element = document.getElementById(elementId);
    if (element === undefined || element === null) {
        return { height: 0, width: 0 }
    }
    return { height: element.offsetHeight, width: element.offsetWidth }
}

function removeSelectedElement(elementId) {
    const element = document.getElementById(elementId);
    element.selectedIndex = 0;
}

function setSelectedElement(elementId, modelIndex) {
    const element = document.getElementById(elementId);
    element.selectedIndex = modelIndex;
}

function handleConfirmation(message) {
    return confirm(message)
}