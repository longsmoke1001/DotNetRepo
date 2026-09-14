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
    <div className="min-h-screen bg-gray-100">
      {token && (
        <nav className="bg-slate-800 text-white px-6 py-4 flex justify-between items-center shadow-md">
          <Link to="/notes" className="font-semibold hover:underline">
            我的筆記
          </Link>
          <button
            onClick={handleLogout}
            className="bg-red-500 hover:bg-red-600 px-4 py-2 rounded transition"
          >
            登出
          </button>
        </nav>
      )}

      <div className="container mx-auto p-6">
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
    </div>
  );
}

export default App;