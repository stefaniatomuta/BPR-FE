const fileName = "Download";

function DownloadResultsToPDF() {
    const elementToPrint = document.createElement('div');
    const elements = document.getElementsByClassName('result-pdf');

    for (let i = 0; i < elements.length; i++) {
        elementToPrint.appendChild(elements[i].cloneNode(true));
    }
    
    const opt = {
        margin: 15,
        filename: fileName,
    };

    html2pdf()
        .set(opt)
        .from(elementToPrint)
        .save();
}

function getElementOffset(elementId) {
    const element = document.getElementById(elementId);
    return { x: element.offsetLeft, y: element.offsetTop }
}

function getElementSizeByClass(className) {
    const element = document.getElementsByClassName(className)[0];
    if (element === undefined) {
        return null
    }
    return { height: element.offsetHeight, width: element.offsetWidth }
}

function removeSelectedElement(elementId) {
    const element = document.getElementById(elementId);
    element.selectedIndex = 0;
}

function handleConfirmation(message) {
    return confirm(message)
}