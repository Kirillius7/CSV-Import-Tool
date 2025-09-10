document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("peopleTable");

    // Inline editing
    table.querySelectorAll(".editable").forEach(td => {
        td.addEventListener("click", () => {
            if (td.querySelector("input") || td.querySelector("select")) return;

            const currentValue = td.innerText;
            if (td.dataset.field === "Married") { // перевірка стовпця 
                const select = document.createElement("select"); // спадний список для сімейного статусу 
                ["True", "False"].forEach(val => {
                    const option = document.createElement("option"); // створення нового пункта на основі відповіді
                    option.value = val; // значення для сервера 
                    option.text = val; // значення для користувача
                    if (val === currentValue) option.selected = true; // співставлення з минулим значенням, вибір за замовченням
                    select.appendChild(option); // додавання пункту у список
                });
                td.innerHTML = ""; // видалення минулої інформації
                td.appendChild(select); // оновлення даних у клітинці 
                select.focus(); // курсор на новий список
            } else {
                const input = document.createElement("input"); // введення даних власноруч
                input.value = currentValue;
                td.innerHTML = "";
                td.appendChild(input);
                input.focus();
            }
        });
    });

    // Saving changes
    table.querySelectorAll(".saveBtn").forEach(btn => {
        btn.addEventListener("click", async () => {
            const tr = btn.closest("tr"); // пошук id користувача, до якого має відношення кнопка зберігання даних
            const id = tr.dataset.id;
            const data = {}; // змінна для збереження даних

            // ітерація через всі клітинки для збереження даних, це стосується як "input" так і "select"
            tr.querySelectorAll(".editable").forEach(td => {
                const field = td.dataset.field;
                let value;
                if (td.querySelector("input")) value = td.querySelector("input").value;
                else if (td.querySelector("select")) value = td.querySelector("select").value;
                else value = td.innerText;
                data[field] = value; // запис значень
            });

            const response = await fetch(`/Person/UpdateRow/${id}`, { // post-запит для збереження даних
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data) // передача даних у форматі JSON
            });

            if (response.ok) { // у разі успішного збереження даних, заміна всіх "input" і "select" на текст
                tr.querySelectorAll(".editable").forEach(td => {
                    const field = td.dataset.field;
                    td.innerText = data[field];
                });
            } else {
                alert("Error saving data");
            }
        });
    });
});
