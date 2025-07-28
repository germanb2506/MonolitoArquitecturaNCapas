function ModalNuevoCliente() {
    $('#modalNuevoCliente').modal('show');
}
document.getElementById("btnGuardarNuevoCliente").addEventListener("click", async function () {
    const clienteDto = {
        razonSocial: document.getElementById("razonSocial").value,
        nit: document.getElementById("nit").value,
        tipoCliente: document.getElementById("tipoCliente").value,
        representanteLegal: document.getElementById("representanteLegal").value,
        correoContacto: document.getElementById("correoContacto").value,
        telefonoContacto: document.getElementById("telefonoContacto").value,
        direccion: document.getElementById("direccion").value,
        ciudad: document.getElementById("ciudad").value,
        pais: document.getElementById("pais").value,
        paginaWeb: document.getElementById("paginaWeb").value,
        notas: document.getElementById("notas").value,
        activo: document.getElementById("activo").checked 
    };


    const urlObjetivo = "/Cliente/Crear"; 

    try {
        const response = await fetch(urlObjetivo, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(clienteDto)
        });

        if (!response.ok) {
            throw new Error(`HTTP ${response.status} - ${response.statusText}`);
        }

        const data = await response.json();
        console.log("Respuesta del servidor:", data);
    } catch (error) {
        console.error("Error al enviar los datos:", error);
    }
});

