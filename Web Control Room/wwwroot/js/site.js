
let sortDirections = [true, true, true, true];

function sortTable(colIndex) {
    const table = document.getElementById("roomsTable");
    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.rows);
    const ascending = sortDirections[colIndex];

    rows.sort((a, b) => {
        let valA = a.cells[colIndex].innerText.trim();
        let valB = b.cells[colIndex].innerText.trim();

        const numA = parseFloat(valA);
        const numB = parseFloat(valB);
        if (!isNaN(numA) && !isNaN(numB)) {
            valA = numA;
            valB = numB;
        }

        return ascending ? valA > valB ? 1 : valA < valB ? -1 : 0
            : valA < valB ? 1 : valA > valB ? -1 : 0;
    });

    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }

    rows.forEach(row => tbody.appendChild(row));
    sortDirections[colIndex] = !ascending;
}
