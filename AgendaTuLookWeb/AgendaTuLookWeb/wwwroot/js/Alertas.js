document.addEventListener("DOMContentLoaded", function () {
    let tempDataDiv = document.getElementById("tempDataMessages");

    if (tempDataDiv) {
        let successMessage = tempDataDiv.dataset.success;
        let errorMessage = tempDataDiv.dataset.error;
        let warningMessage = tempDataDiv.dataset.warning;

        if (successMessage) {
            Swal.fire({
                title: "Éxito",
                text: successMessage,
                icon: "success"
            });
        }

        if (errorMessage) {
            Swal.fire({
                title: "Error",
                text: errorMessage,
                icon: "error"
            });
        }

        
        if (warningMessage) {
            Swal.fire({
                title: "Atención",
                text: warningMessage,
                icon: "warning"
            });
        }
    }
});