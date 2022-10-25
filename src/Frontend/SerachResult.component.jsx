import React from 'react'
import { useLocation } from 'react-router-dom'

const SearchResult = () =>{

    let checkIfSearchWork = true;
    var from = ""; 
    const location = useLocation()
    if (location.state === undefined || location.state === null || location.state.from.length === 0) {

        from = ""
        checkIfSearchWork = false

    }
    else{
        console.log(location.state.from.length);

        from = location.state

    }


    var matches = from.match(/\d+/g);
    if (matches != null) {
        checkIfSearchWork = false;
    }

    if(checkIfSearchWork){
        return (
            <form>
                <div>
                <h1>{from}</h1>
                </div>
            </form>)
    }
    else{
        return (
            <form>
                <div>
                    <h1>No search result found</h1>
                </div>
            </form>)
    }

}

export default SearchResult;