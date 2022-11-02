
const SignUpUser = async (json) => {
  return new Promise(function(resolve, reject) {
    const Http = new XMLHttpRequest();
    const url='http://localhost:7071/api/HttpTiggerSignUp';
    Http.open("POST", url);
    Http.setRequestHeader('Access-Control-Allow-Origin', '*');
    Http.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    Http.send(JSON.stringify(json));     
    Http.onreadystatechange = (e) => {
      try {
        console.log(Http.responseText);
        if(Http.responseText.length > 5){
        const obj = JSON.parse(Http.responseText);
        resolve(obj);
        }
        

      } catch (error) {
        reject(error);
      }

    }
  })    
};
export default SignUpUser;