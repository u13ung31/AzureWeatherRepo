
const SignUpUser = async (json) => {

    const sql = require('mssql')
    try {
        // make sure that any items are correctly URL encoded in the connection string
        await sql.connect('Server=localhost; Database=AzureWeatherDB;Trusted_Connection=True')
        // Finns användaren redan 
        const result = await sql.query`insert into Users(UserName, Password) values(json.UserName, json.Password)`
        console.dir(result)
    } catch (err) {
        // ... error checks
    }
};