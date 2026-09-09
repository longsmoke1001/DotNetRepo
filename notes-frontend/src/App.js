import { useState } from 'react';
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
      {!token ? (
        <Login onLoginSuccess={handleLoginSuccess} />
      ) : (
        <div>
          <button onClick={handleLogout} style={{ float: 'right', margin: 10 }}>
            登出
          </button>
          <NotesList token={token} />
        </div>
      )}
    </div>
  );
}

export default App;