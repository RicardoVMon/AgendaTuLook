Dropzone.autoDiscover = false;

var myDropzone = new Dropzone("#dropzoneArea", {
    url: "#", // No lo usamos, el formulario se enviará normalmente
    autoProcessQueue: false, // No subimos automáticamente
    maxFiles: 1, // Permitir hasta 5 archivos
    acceptedFiles: null, // Acepta cualquier tipo de archivo
    addRemoveLinks: true,
    dictDefaultMessage: "Por favor, arrastre el comprobante del SINPE a este campo",
    dictRemoveFile: "Eliminar archivo",

    init: function () {
        var dropzoneInstance = this;

        // Evento cuando se agrega un archivo
        this.on("addedfile", function (file) {
            if (this.files.length > 1) {
                this.removeFile(this.files[0]); // Elimina el archivo más antiguo si excede el límite
            }

            // Asignar archivos al input file
            var fileInput = document.getElementById("Archivos");
            var dataTransfer = new DataTransfer();

            // Agregar todos los archivos de Dropzone al input file
            this.files.forEach(f => dataTransfer.items.add(f));

            fileInput.files = dataTransfer.files;
        });

        // Evento para eliminar archivo
        this.on("removedfile", function () {
            var fileInput = document.getElementById("Archivos");
            var dataTransfer = new DataTransfer();

            // Agregar los archivos restantes después de eliminar uno
            this.files.forEach(f => dataTransfer.items.add(f));

            fileInput.files = dataTransfer.files;
        });
    }
});

// Permitir pegar archivos con Ctrl + V
document.addEventListener("paste", function (event) {
    var items = event.clipboardData.items;
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        if (item.kind === "file") {
            var file = item.getAsFile();
            if (file) {
                if (myDropzone.files.length < 1) {
                    myDropzone.addFile(file);
                }
            }
        }
    }
});