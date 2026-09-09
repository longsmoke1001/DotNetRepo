import { useState, useEffect } from 'react';
import axios from 'axios';

function NotesList({ token }) {
  const [notes, setNotes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchNotes = async () => {
      try {
        const response = await axios.get('https://localhost:5001/api/notes', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        setNotes(response.data);
      } catch (err) {
        console.error('Fetch notes error:', err);
        alert('拎唔到 Notes，請確認 Token 有效');
      } finally {
        setLoading(false);
      }
    };

    fetchNotes();
  }, [token]);

  if (loading) return <p>載入中...</p>;

  return (
    <div>
      <h2>我的筆記</h2>
      {notes.length === 0 ? (
        <p>暫時冇筆記</p>
      ) : (
        <ul>
          {notes.map((note) => (
            <li key={note.id} style={{ padding: '8px 0', borderBottom: '1px solid #eee' }}>
              <strong>{note.title}</strong>
              <p style={{ margin: '4px 0 0 0', color: '#666', fontSize: 14 }}>
                {note.content}
              </p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default NotesList;