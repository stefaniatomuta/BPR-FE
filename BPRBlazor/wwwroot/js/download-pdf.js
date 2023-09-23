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