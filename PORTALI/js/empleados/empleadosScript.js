const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 5000,
    timerProgressBar: true
})

const empleados = @Html.Raw(Json.Encode(Model.Empleados));
const modalPosiciones = new bootstrap.Modal(document.getElementById('modalPosiciones'));
const modalEmpleado = new bootstrap.Modal(document.getElementById('modalEmpleado'));
const modalPerfilPuesto = new bootstrap.Modal(document.getElementById('modalPerfilPuesto'));
const modalDepartamentos = new bootstrap.Modal(document.getElementById('modalDepartamentos'));
const modalEquipos = new bootstrap.Modal(document.getElementById('modalEquipos'));

let resultado = [];
const containerBody = document.getElementById('bodyEmpleados');

function filtrarPorNombre() {
    const nombre = document.getElementById('filtroNombre').value
    const estado = document.getElementById('filtroEstado').value

    if (nombre.length > 2) {
        if (estado === "Y" || estado === "N") {
            resultado = empleados.filter(e =>
                e.Nombre.toLowerCase().includes(nombre.toLowerCase()) && e.Activo == estado
            );
        }
        else {
            resultado = empleados.filter(e =>
                e.Nombre.toLowerCase().includes(nombre.toLowerCase())
            );
        }
    }
    else {
        if (estado === "Y" || estado === "N") {
            resultado = empleados.filter(e => e.Activo == estado);
        }
        else {
            resultado = empleados;
        }
    }

    document.getElementById('resultadosEncontrados').innerText = resultado.length
    let body = '';
    resultado.forEach(empleado => {
        body += `
                        <tr>
                            <td>${empleado.EmpId}</td>
                            <td>${empleado.Nombre}</td>
                            <td>${empleado.Puesto}</td>
                            <td>${empleado.Departamento}</td>
                            <td>
                                ${empleado.Activo === "Y"
                ? '<span class="badge bg-success">Activo</span>'
                : '<span class="badge bg-danger">Inactivo</span>'
            }
                            </td>
                            <td>${empleado.JefeInmediato}</td>
                            <td>
                                        <button class="btn btn-sm btn-primary" title="Ver perfil" onclick="abrirModalEmpleado(${empleado.EmpId})">
                                            <i class="bi bi-person-vcard"></i>
                                        </button>
                            </td>
                        </tr>`;
    });
    containerBody.innerHTML = body;
}

function abrirModalPosiciones() {
    modalPosiciones.show();
}

function actualizarContador() {
    const input = document.getElementById("posNombre");
    const contador = document.getElementById("contadorPosNombre");
    const max = input.maxLength;
    const usados = input.value.length;
    const restantes = max - usados;

    contador.textContent = `${restantes} caracteres restantes`;

    if (restantes <= 5) {
        contador.classList.remove("text-muted");
        contador.classList.add("text-danger");
    } else {
        contador.classList.remove("text-danger");
        contador.classList.add("text-muted");
    }
}

async function guardarPosicion() {
    if (!document.getElementById('formPosicion').checkValidity()) {
        Toast.fire({ icon: 'warning', title: "Todos los capos son obligatorios" });
        return;
    }
    $("#globalLoading").show();

    try {
        const nombre = document.getElementById('posNombre').value;
        const descripcion = document.getElementById('posDescripcion').value;

        const data = new FormData();
        data.append("nombre", nombre);
        data.append("posicion", descripcion);

        const response = await fetch('/Empleados/GuardarPosicion', {
            method: "Post",
            body: data
        })

        const result = await response.json();

        if (result.success) {
            Toast.fire({ icon: 'success', title: "Posicion guardada con éxito" });

            body = `<tr>
                                        <th>${result.positionID}</th>
                                        <th>${nombre.toUpperCase()}</th>
                                        <th>${descripcion.toUpperCase()}</th>
                                    </tr>`;

            option = `<option value="${result.positionID}">${descripcion.toUpperCase()}</option>`;
            document.getElementById('posicionesBody').insertAdjacentHTML("beforeend", body);
            document.getElementById('position').insertAdjacentHTML("beforeend", option);
        }
        else {
            Toast.fire({ icon: 'error', title: "Ocurrio un error al guardar la posición" });
        }
    }
    catch (error) {
        Toast.fire({ icon: 'error', title: "Ocurrio un error al guardar la posición" });
        console.log(error);
    }
    finally {
        $("#globalLoading").hide();
    }
}

async function abrirModalEmpleado(empId) {
    $("#globalLoading").show();
    const response = await fetch(`/Empleados/ObtenerInforacionEmpleado?empId=${empId}`);
    const result = await response.json();

    try {
        if (result.success) {
            console.log(result);
            document.getElementById('firstName').value = result.data.firstName ?? ""
            document.getElementById('middleName').value = result.data.middleName ?? ""
            document.getElementById('lastName').value = result.data.lastName ?? ""
            document.getElementById('jobTitle').value = result.data.jobTitle ?? ""
            document.getElementById('position').value = result.data.position ?? ""
            document.getElementById('dept').value = result.data.dept ?? ""
            document.getElementById('empID').value = result.data.empID ?? ""
            document.getElementById('Active').checked = result.data.Active == 'Y' ? true : false;
            document.getElementById('mobile').value = result.data.mobile ?? ""
            document.getElementById('hometel').value = result.data.hometel ?? ""
            document.getElementById('email').value = result.data.Email ?? ""
            document.getElementById('U_JefeInmediato').value = result.data.U_JefeInmediato ?? ""
            document.getElementById('U_tienda').value = result.data.U_Tienda ?? ""

            console.log()

            console.log(result.data.U_Tienda);
            modalEmpleado.show();
        }
        else {
            Toast.fire({ icon: 'warning', title: "Ocurrio un error al cargar la información del empleado" });
        }
    }
    catch (error) {
        Toast.fire({ icon: 'error', title: "Ocurrio un error al cargar la información del empleado" });
    }
    finally {
        $("#globalLoading").hide();
    }
}

async function guardarDatosEmpleado() {
    $("#globalLoading").show();

    try {
        const datosEmpleado = {
            FirstName: document.getElementById('firstName').value,
            MiddleName: document.getElementById('middleName').value,
            LastName: document.getElementById('lastName').value,
            JobTitle: document.getElementById('jobTitle').value,
            Position: document.getElementById('position').value,
            Department: document.getElementById('dept').value,
            EmpID: document.getElementById('empID').value,
            Active: document.getElementById('Active').checked ? 'Y' : 'N',
            MobilePhone: document.getElementById('mobile').value,
            HomePhone: document.getElementById('hometel').value,
            eMail: document.getElementById('email').value,
            U_JefeInmediato: document.getElementById('U_JefeInmediato').value,
            U_Tienda: document.getElementById('U_tienda').value
        };

        const response = await fetch('/Empleados/GuardarInformacionEmpleado',
            {
                method: "POST",
                body: JSON.stringify({ infoEmpleado: datosEmpleado }),
                headers: { "Content-Type": "application/json" }
            }
        );
        const result = await response.json();
        if (result.success) {
            modalEmpleado.hide();
            $("#globalLoading").hide();

            Toast.fire({ icon: 'success', title: "El usuario ha sido actulizado en la base de datos de SAP" });
            setTimeout(() => {
                location.reload();
            }, 2000);
        }
        else {
            Toast.fire({ icon: 'error', title: "Ocurrio un error al actualizar al usuario" });
        }
    }
    catch (error) {
        console.log(error);
        Toast.fire({ icon: 'error', title: "Ocurrio un error al actualizar al usuario" });
    }
    finally {
        $("#globalLoading").hide();
    }
}
document.getElementById("posNombre").addEventListener("input", function () {
    this.value = this.value.trimStart();
});

document.getElementById('filtroNombre').addEventListener('input', () => filtrarPorNombre());
document.getElementById('filtroEstado').addEventListener('change', () => filtrarPorNombre());
document.getElementById('btnGuardarEmpleado').addEventListener('click', () => guardarDatosEmpleado());
document.getElementById('btnPuestos').addEventListener('click', () => abrirModalPosiciones());
document.getElementById('btnAgregarPosicion').addEventListener('click', (e) => {
    e.preventDefault();
    guardarPosicion();
});

async function abrirModalPerfilPuesto(puesto, puestoId, perfil, idButton) {
    document.getElementById('tituloPuesto').innerText = puesto;
    document.getElementById('formPerfilPuesto').reset();
    bloquearFormulario('formPerfilPuesto', false);
    document.getElementById('btnGuardarPerfil').classList.remove("d-none")
    document.getElementById('btnAbilitarEditar').classList.add("d-none")
    document.getElementById('U_IdPuesto').value = puestoId;
    console.log(idButton);
    document.getElementById('U_IdButton').value = idButton;

    const response = await fetch(`/Reclutamiento/ObtenerPerfilPuestoPorId?idPuesto=${puestoId}`);
    const result = await response.json();

    if (result.success) {
        bloquearFormulario('formPerfilPuesto', true);
        document.getElementById('btnGuardarPerfil').classList.add("d-none")
        document.getElementById('btnAbilitarEditar').classList.remove("d-none")

        document.getElementById('U_ExperienciaMinima').value = result.data.U_ExperienciaMinima
        document.getElementById('U_NivelEstudio').value = result.data.U_NivelEstudio
        document.getElementById('U_Observaciones').value = result.data.U_Observaciones
        document.getElementById('U_RangoEdadMax').value = result.data.U_RangoEdadMax
        document.getElementById('U_RangoEdadMin').value = result.data.U_RangoEdadMin
        document.getElementById('U_SalarioMax').value = result.data.U_SalarioMax
        document.getElementById('U_SalarioMin').value = result.data.U_SalarioMin
        document.getElementById('U_Sexo').value = result.data.U_Sexo
        document.getElementById('Code').value = result.data.Code
    }
    modalPerfilPuesto.show();
}

async function guardarPerfilDeUsuario() {
    if (!document.getElementById('formPerfilPuesto').checkValidity()) {
        Toast.fire({ icon: 'warning', title: "Debe llenar los campos obligarorios" });
        return;
    }

    $("#globalLoading").show();
    const idButton = document.getElementById('U_IdButton').value;
    const button = document.getElementById(idButton);
    const icono = document.getElementById("i" + idButton);
    try {
        const perfilUsuario = {
            U_IdPuesto: document.getElementById('U_IdPuesto').value,
            U_ExperienciaMinima: document.getElementById('U_ExperienciaMinima').value,
            U_NivelEstudio: document.getElementById('U_NivelEstudio').value,
            U_Observaciones: document.getElementById('U_Observaciones').value,
            U_RangoEdadMax: document.getElementById('U_RangoEdadMax').value,
            U_RangoEdadMin: document.getElementById('U_RangoEdadMin').value,
            U_SalarioMax: document.getElementById('U_SalarioMax').value,
            U_SalarioMin: document.getElementById('U_SalarioMin').value,
            U_Sexo: document.getElementById('U_Sexo').value,
            Code: document.getElementById('Code').value
        };

        const response = await fetch('/Reclutamiento/GuardarPerfilPuesto', {
            method: 'POST',
            body: JSON.stringify({ perfil: perfilUsuario }),
            headers: { "Content-Type": "application/json" }
        });

        const result = await response.json();

        if (result.success) {
            button.classList.add('btn-outline-success');
            button.classList.remove('btn-outline-danger');
            button.setAttribute('title', 'Ver perfil');
            icono.classList.add('text-success');
            icono.classList.remove('text-danger');
            Toast.fire({ icon: 'success', title: "Perfil guardado con éxito " });
            modalPerfilPuesto.hide();
        }
        else {
            Toast.fire({ icon: 'error', title: "Ocurrio un error al guardar el perfil" });
        }
    }
    catch (error) {
        Toast.fire({ icon: 'error', title: "Ocurrio un error al guardar el perfil" });
        console.log(error);
    }
    finally {
        $("#globalLoading").hide();
    }
}

function bloquearFormulario(formId, bloquearlo) {
    const form = document.getElementById(formId);
    const elementos = form.querySelectorAll("input, select, textarea, button");

    elementos.forEach(el => {
        el.disabled = bloquearlo;
    });
}
function abilitarEditar() {
    bloquearFormulario('formPerfilPuesto', false);
    document.getElementById('btnAbilitarEditar').classList.add("d-none")
    document.getElementById('btnGuardarPerfil').classList.remove("d-none")
}

function abrirModalDepartamentos() {
    modalDepartamentos.show();
}

async function cambio(select, DeptoId) {
    try {
        const idSelect = `selectGerenteDepartamento${DeptoId}`
        const selectGerente = document.getElementById(idSelect).value;

        console.clear();
        console.log("Gerente : ", selectGerente);
        console.log("Departamento : ", DeptoId);
        const data = new FormData();
        data.append("gerente", selectGerente);
        data.append("departamento", DeptoId);

        const response = await fetch('/Empleados/GuardarGerenteDepartamento',
            {
                method: "POST",
                body: data
            }
        );

        const result = await response.json();

        if (result.success) {
            Toast.fire({ icon: 'success', title: "Gerente actualizado" });
        }
        else {
            Toast.fire({ icon: 'error', title: "Ocurrio un error al actualizar el gerente" });
        }
    }
    catch {
        Toast.fire({ icon: 'error', title: "Ocurrio un error al actualizar el gerente" });
    }
}

//AGREGAR EQUIPOS INICIO

function renderListas() {
    const disponibles = document.getElementById("listaDisponibles");
    const asignados = document.getElementById("listaAsignados");

    disponibles.innerHTML = "";
    asignados.innerHTML = "";

    equiposDisponibles.forEach(eq => {
        disponibles.appendChild(crearEquipoItem(eq));
    });

    equiposAsignados.forEach(eq => {
        asignados.appendChild(crearEquipoItem(eq));
    });
}

function crearEquipoItem(equipo) {
    const div = document.createElement("div");
    div.classList.add("equipo-item");
    div.draggable = true;
    div.dataset.id = equipo.id;

    div.innerHTML = `
    <i class="bi bi-laptop"></i>
    <span>${equipo.nombre}</span>
  `;

    div.addEventListener("dragstart", () => {
        div.classList.add("dragging");
    });

    div.addEventListener("dragend", () => {
        div.classList.remove("dragging");
    });

    return div;
}

document.querySelectorAll(".lista-equipos").forEach(lista => {
    lista.addEventListener("dragover", e => {
        e.preventDefault();
        const dragging = document.querySelector(".dragging");
        lista.appendChild(dragging);
    });

    lista.addEventListener("drop", e => {
        const id = parseInt(document.querySelector(".dragging").dataset.id);

        if (lista.id === "listaAsignados") {
            moverEquipo(id, equiposDisponibles, equiposAsignados);
        } else {
            moverEquipo(id, equiposAsignados, equiposDisponibles);
        }

        renderListas();
    });
});

function moverEquipo(id, origen, destino) {
    const index = origen.findIndex(e => e.id === id);
    if (index >= 0) {
        destino.push(origen[index]);
        origen.splice(index, 1);
    }
}

// Inicializar

let equiposDisponibles = [
    { id: 1, nombre: "Laptop" },
    { id: 2, nombre: "Celular" },
    { id: 3, nombre: "Tablet" },
    { id: 4, nombre: "Monitor" },
    { id: 5, nombre: "Impresora" }
];

let equiposAsignados = [];

function verEquiposAsignados() {
    renderListas();
    modalEquipos.show()
}

//AGREGAR EQUIPO FIN

document.getElementById('btnGuardarPerfil').addEventListener('click', () => guardarPerfilDeUsuario());
document.getElementById('btnAbilitarEditar').addEventListener('click', () => abilitarEditar());
document.getElementById('btnDepartamentos').addEventListener('click', () => abrirModalDepartamentos());
document.getElementById('btnVerEquipo').addEventListener('click', () => verEquiposAsignados());