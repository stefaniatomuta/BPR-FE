async function TransformToPng()
{
    const elements = document.getElementsByClassName('pdf-image')
    for (let i = 0; i < elements.length; i++) {
        const img = new Image()
        img.src = await htmlToImage.toPng(elements[i].firstElementChild)
        elements[i].firstElementChild.remove()
        elements[i].appendChild(img)
    }
}

function DownloadResultsToPDF(){
    window.focus()
    window.print()
    location.reload();
}

function getElementOffset(elementId) {
    const element = document.getElementById(elementId);
    return { x: element.offsetLeft, y: element.offsetTop }
}

function getElementSize(elementId) {
    const element = document.getElementById(elementId)
    if (element === undefined || element === null) {
        return { height: 0, width: 0 }
    }
    return { height: element.offsetHeight, width: element.offsetWidth }
}

function removeSelectedElement(elementId) {
    const element = document.getElementById(elementId)
    element.selectedIndex = 0;
}

function setSelectedElement(elementId, modelIndex) {
    const element = document.getElementById(elementId)
    element.selectedIndex = modelIndex;
}

function handleConfirmation(message) {
    return confirm(message)
}