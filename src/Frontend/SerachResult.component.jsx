import React,{useState,useEffect} from 'react';
import { useLocation } from 'react-router-dom'
import './SearchBox.Styling.css'
import { SearchResultJS } from '../Backend/Services/SearchResultQueryFinder';
import { AddFavoritePlace } from '../Backend/DataServices/AddFavoritePlace';
import { useContext } from "react";
import { UserContext } from '../App';
import { useNavigate } from 'react-router-dom';

const SearchResult = () =>{
    const Loc = useLocation();

    const[DataisLoaded,SetDataLoaded] = useState(false);
    const[items,SetItems] = useState([]);
    const {user} = useContext(UserContext);
    const navigate = useNavigate();

    useEffect(() => {
        let isMounted = true;               

        SearchResultJS(Loc.state.from).then(json =>{
            if (isMounted){
                console.log(json);
                SetItems(json)
                SetDataLoaded(true)

            }
        })
      }, [Loc]);
    
    const AddButton = (data) =>{
        var JsonCity ={
            Latitude: data.lat,
            Longitude: data.lon,
            CityName: data.name,
            UserId: user.UserID 
        }

        AddFavoritePlace(JsonCity).then(res =>{
            navigate("/");
        })


    }
   
    let checkIfSearchWork = true;
    var from = ""; 
    const location = useLocation()
    if (location.state === undefined || location.state === null || location.state.from.length === 0) {

        from = ""
        checkIfSearchWork = false

    }
    else{

        from = location.state.from

    }


    var matches = from.match(/\d+/g);
    if (matches != null) {
        checkIfSearchWork = false;
    }
    
    if(checkIfSearchWork && DataisLoaded){
        const places = []
        let countDown = 0;
        items.forEach(data =>{
            places.push(
                <form className='FormSearch' key={countDown}>
                    <div>
                        <div>
                            <h4>{data.name}</h4>
                        </div>
                        
                        <div>
                            <p>{data.country} - {data.state}</p>
                        </div>
                    </div>
                    <div className='DivLink'>
                        <button type="button" onClick={() => {
                        AddButton(data);
                        }}>                        
                            <i className="fa fa-star"></i>
                        </button>
                    </div>
                   

                </form>
            )
            countDown++;
        });

        return (
            <div>
            {places}
            </div>)          
    }
    else{
        return ( <form>
            <div>
                <h1>No search result found</h1>
            </div>
        </form>) ;
    }

}

export default SearchResult;