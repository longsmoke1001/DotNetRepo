import { useState } from 'react';
import { Routes, Route, Navigate, Link } from 'react-router-dom';
import Login from './Login';
import NotesList from './NotesList';
import './App.css';

function App() {
  const [token, setToken] = useState(localStorage.getItem('token') || null);

  const handleLoginSuccess = (newToken) => {
    setToken(newToken);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken(null);
  };

  return (
    <div className="App">
      {token && (
        <nav style={{ padding: 10, borderBottom: '1px solid #ccc' }}>
          <Link to="/notes" style={{ marginRight: 15 }}>My Notes</Link>
          <button onClick={handleLogout} style={{ float: 'right' }}>Log Out</button>
        </nav>
      )}

      <Routes>
        <Route
          path="/"
          element={
            token ? <Navigate to="/notes" /> : <Login onLoginSuccess={handleLoginSuccess} />
          }
        />
        <Route
          path="/notes"
          element={
            token ? <NotesList token={token} /> : <Navigate to="/" />
          }
        />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </div>
  );
}

export default App;