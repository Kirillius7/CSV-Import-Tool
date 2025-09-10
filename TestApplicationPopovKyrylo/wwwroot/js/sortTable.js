
document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("peopleTable");
    const filterInput = document.getElementById("filterInput");

    if (!table) return;

    // Filter 
    if (filterInput) {
        filterInput.addEventListener("keyup", () => { // реакція на кожну дію користувача під час роботи з клавішами
            const filter = filterInput.value.toLowerCase();
            table.querySelectorAll("tbody tr").forEach(row => { // кожний рядок таблиці перетворюється у масив для 
                //подальшого перегляду наявності тексту з рядка фільтрації
                const cells = Array.from(row.cells);
                const match = cells.some(td => td.innerText.toLowerCase().includes(filter));
                row.style.display = match ? "" : "none"; // у разі наявності такої клітинки - відображення рядка
            });
        });
    }

    // Sort
    table.querySelectorAll("th").forEach((th, colIndex) => { // вибір всіх заголовків таблиці
        th.style.cursor = "pointer"; // активний курсор для роботи
        th.addEventListener("click", () => {
            const rows = Array.from(table.querySelectorAll("tbody tr")); // збір всіх рядків таблиці для сортування
            const asc = th.dataset.asc === "true"; // вибір у якому порядку (на зменшення/збільшення значень)
            rows.sort((a, b) => { // сортування рядків таблиці за індексом обраного стовпця 
                let aText = a.cells[colIndex].innerText.trim(); // вилучення зайвих пробілів
                let bText = b.cells[colIndex].innerText.trim();

                // порівняння чисел
                if (!isNaN(aText) && !isNaN(bText)) { // перевірка значень, що вони є числами
                    return asc ? aText - bText : bText - aText; // якщо на зменшення - більше число йде першим і навпаки
                }

                // порівняння дати
                if (!isNaN(Date.parse(aText)) && !isNaN(Date.parse(bText))) { // перетворення текста в дату
                    return asc ? new Date(aText) - new Date(bText) : new Date(bText) - new Date(aText);
                }

                // otherwise comparing text
                return asc
                    ? aText.localeCompare(bText) // порівняння на основі алфавітного порядку з урахуванням локалі.
                    : bText.localeCompare(aText);
            });

            th.dataset.asc = !asc; // якщо користувач наступний раз натисне на стовпець - він буде сортуватись за різним
            // порядком (asc -> desc, desc -> asc)
            rows.forEach(row => table.querySelector("tbody").appendChild(row));
            // зображення відсортованих рядків у таблиці, завантаження йде з кінця у новому порядку
        });
    });
});


