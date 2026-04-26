
// let expenses = [];

// function getExpenses() {
//     fetch(uri)
//     .then(response => response.json())
//     .then(data => _displayItems(data))
//     .catch(error => console.error("unable to get expenses", error))
// }

async function getExpenses() {
    const url = "https://localhost:7060/api/expenses";
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }

        const data = await response.json();
        console.log(data)
        
        // const obj1 = (data[0].name);
        // document.getElementById('name').innerHTML = obj1;

        displayExpenses(data);
    } catch (error) {
        console.error(error.message);
    }
}

function displayExpenses(data) {
    var expenses = document.getElementById('expenses');
    // document.getElementById('expenses').innerHTML = "";

    data.forEach(expense => {
        const row = document.createElement("tr");
        // row.id = "row";

        const name = document.createElement("td");
        name.textContent = expense.name;

        const amount = document.createElement("td");
        amount.textContent = expense.amount;

        const date = document.createElement("td");
        date.textContent = expense.date;
        
        //dodac przyciski akcji dla kazdego (edytuj, usun)

        row.appendChild(name);
        row.appendChild(amount);
        row.appendChild(date)
        expenses.appendChild(row);

        // const obj1 = (data[1].name);
        // document.getElementById('name').innerHTML = obj1;
    })
}

console.log();
getExpenses();
// _displayItems(data);