const fileName = "Download";

function DownloadResultsToPDF() {
    const element = document.getElementById('result-details');

    const opt = {
        margin: 10,
        filename: fileName,
    };

    html2pdf()
        .set(opt)
        .from(element)
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
    var element = document.getElementById(elementId);
    element.selectedIndex = 0;
}

function handleConfirmation(message) {
    return confirm(message)
}