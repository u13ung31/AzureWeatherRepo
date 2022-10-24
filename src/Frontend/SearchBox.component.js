import React, { Component } from 'react'
import './SearchBox.Styling.css'
import {  Link } from 'react-router-dom'

export default class SearchBox extends Component {
  render() {
    return (
        <form className='topnav'>
        <input type="text" placeholder="Search.." name="search"/>
        <Link to={'/sign-up'}>
            <button><i className="fa fa-search"></i></button>
        </Link>
      </form>
    )
  }
}