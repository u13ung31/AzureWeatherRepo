import React,{useState,useEffect,useRef} from 'react';
import './SearchBox.Styling.css'
import { GetFavoritePlace } from '../Backend/DataServices/GetFavoriteWeather';
import { DeletePlace } from '../Backend/DataServices/DeletePlace';
import { useContext } from "react";
import { UserContext } from '../App';

const SearchResult = () =>{
    const[DataisLoaded,SetDataLoaded] = useState(false);
    const[items,SetItems] = useState([]);
    const {user} = useContext(UserContext);

useEffect(() => {
    
    let isMounted = true;
    GetFavoritePlace(user).then(json =>{
        if (isMounted){
            console.log(json);
            SetDataLoaded(true);
            SetItems(json)
        }
    });
    const interval = setInterval(() => {
    let isMounted = true;
    SetDataLoaded(false);

    if(!DataisLoaded){
        GetFavoritePlace(user).then(json =>{
            if (isMounted){
                console.log(json);
                SetDataLoaded(true);
                SetItems(json)
            }
        });
    }
    }, 240000);
    return () => clearInterval(interval);
  }, []);

    
    const DeleteButton = (ID) =>{

        DeletePlace(ID)
    }
   
    if(DataisLoaded){
        const places = []
        let countDown = 0;
        console.log(items.length)
        if(items.length === 0){
            places.push(
            <form className='FormSearch'>
                    <h1>You have no weathers <br/> in your List</h1>        
            </form>)
        }
        else{
            items.forEach(data =>{
                let weatherData;
                if(data.CurrentWeather === null || data.Temperature === null ||
                    data.CelsiusTemp === null ){
                        weatherData = <p>Data yet to be updated</p>;
                }
                else{
                    weatherData =<p>{data.CurrentWeather} - {data.Temperature}K - {data.CelsiusTemp}°C</p>;
                }
                places.push(
                    <form className='FormSearch' key={countDown}>
                        <div>
                            <div>
                                <h4>{data.CityName}</h4>
                            </div>
                            
                            <div>
                                <p>{weatherData}</p>
                            </div>
                        </div>
                        <div className='DivLink'>
                                <button type="button" onClick={() => {
                                DeleteButton(data.FavoriteWeatherId);
                                }}>                        
                                    <i className="fa fa-trash"></i>
                                </button>
                        </div>
                    </form>
                )
                countDown++;
            });
        }
        return (
            <div>
            {places}
            </div>)          
    }
    else{
        return ( <form>
            <div>
                <h1>Data is being fetch</h1>

            </div>
        </form>) ;
    }

}

export default SearchResult;