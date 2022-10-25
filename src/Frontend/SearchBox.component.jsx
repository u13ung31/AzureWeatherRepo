import React from 'react'
import './SearchBox.Styling.css'
import {  Link } from 'react-router-dom'
import {useState} from 'react';

const SearchBox = () =>{
  const [searchQuery, setSearchQuery] = useState('');

  const handleChange = event => {
    setSearchQuery(event.target.value);

    console.log('value is:', event.target.value);
  };

  return (
    <form className='topnav'>
    <input type="text" 
    placeholder="Search.." 
    value={searchQuery}  
    name="search"
    id="search"
    onChange={handleChange}
    />
    <Link to="/Search-Result" state={{ from: searchQuery }}>
        <button><i className="fa fa-search"></i></button>
    </Link>
  </form>)

}

export default SearchBox;