

const Login = async (json) => {

    // kalla på databas
    const sql = require('mssql')
    try {
        // make sure that any items are correctly URL encoded in the connection string
        await sql.connect('Server=localhost; Database=AzureWeatherDB;Trusted_Connection=True')
        const result = await sql.query`select * from Users where Username = ${json.UserName}`
        console.dir(result)
    } catch (err) {
        // ... error checks
    }

};