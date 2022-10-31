
import ImportantString from "../../Enum/ImportantStrings.json";


const CleanTheCityString = (stringValue) =>{
    var separateWord = stringValue.toLowerCase().split(' ');
    for (var i = 0; i < separateWord.length; i++) {
       separateWord[i] = separateWord[i].charAt(0).toUpperCase() +
       separateWord[i].substring(1);
    }
    String(separateWord).trim();
    return separateWord.join(' ');
}

const SearchResultJS = async (QueryString) =>{

    //let CorrectThestring = QueryString.toLowerCase()
    var NewCityName = CleanTheCityString(QueryString);

    var queryCityName = String(NewCityName).replaceAll(" ","%20")
    return new Promise(function(resolve, reject) {

        if(queryCityName !==""){
             fetch("https://api.openweathermap.org/geo/1.0/direct?q="+queryCityName+"&limit=5&appid="+ImportantString.APIKEY).then((res) => res.json())
             .then((json) => {
                 resolve(json)
             }).catch(rej =>{
                console.log(rej);
                reject(rej);
             })
        }
        else{
            reject("Empty string")
        }
    })
}


export {SearchResultJS};