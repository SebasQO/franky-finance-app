document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const nameField = document.getElementById("name");
    const emailField = document.getElementById("email");
    const passwordField = document.getElementById("password");
    const confirmPasswordField = document.getElementById("confirm-password");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        // Validar nombre
        if (nameField.value.trim() === "") {
            isValid = false;
            alert("Name is required.");
        }

        // Validar correo electrónico
        if (!validateEmail(emailField.value)) {
            isValid = false;
            alert("Please enter a valid email address.");
        }

        // Validar contraseña
        if (passwordField.value.length < 6) {
            isValid = false;
            alert("Password must be at least 6 characters long.");
        }

        // Validar confirmación de contraseña
        if (passwordField.value !== confirmPasswordField.value) {
            isValid = false;
            alert("Passwords do not match.");
        }

        // Prevenir envío si hay errores
        if (!isValid) {
            event.preventDefault();
        }
    });

    // Función para validar correos electrónicos
    function validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
});
