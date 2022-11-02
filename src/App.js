import React from 'react'
import { useState } from "react";
import '../node_modules/bootstrap/dist/css/bootstrap.min.css'
import './App.css'
import { BrowserRouter as Router, Routes, Route, Link,Navigate } from 'react-router-dom'
import Login from './Frontend/login.component'
import SignUp from './Frontend/signup.component'
import SearchBox from './Frontend/SearchBox.component'
import SearchResult from './Frontend/SerachResult.component'
import background from "./Img/BgWeather.png";
import WeatherBox from "./Frontend/WeatherBox.component"
export const UserContext = React.createContext();

function App() {
  const [user, setUser] = useState({UserID:0});

  return (
    <UserContext.Provider  value={{user,setUser}}>
    <Router style={{ backgroundImage: `url(${background})` }}>
      <div className="App">
        <nav className="navbar navbar-expand-lg navbar-light fixed-top">
          <div className="container">
            <Link className="navbar-brand" to={'/'}>
              positronX
            </Link>
            <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
              <ul className="navbar-nav ulExtended ml-auto">
                <li className="nav-item">
                  <Link className="nav-link" to={'/sign-in'}>
                    Login
                  </Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link" to={'/sign-up'}>
                    Sign up
                  </Link>
                </li>
                <li className="nav-item">
                  <SearchBox />
                </li>
              </ul>
            </div>
          </div>
        </nav>
        <div className="auth-wrapper">
          <div className="auth-inner">
            <Routes>
              <Route exact path="/" element={user.UserID === 0 ? <Login /> : <WeatherBox /> } />
              <Route path="/sign-in" element={<Login />} />
              <Route path="/sign-up" element={<SignUp />} />
              <Route path="/Search-Result" element={user.UserID !== 0 ? <SearchResult /> : <Navigate to='/'/>} />
              <Route path="/Home"  element={user.UserID !== 0 ? <WeatherBox /> : <Navigate to='/'/>} />
            </Routes>
          </div>
        </div>
      </div>
    </Router>
    </UserContext.Provider>
  )
}
export default App