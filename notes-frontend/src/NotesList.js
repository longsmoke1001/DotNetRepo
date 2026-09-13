import { useState, useEffect } from 'react';
import axios from 'axios';
import AddNote from './AddNote';   // ← 新增：引入 AddNote 元件

function NotesList({ token }) {
  const [notes, setNotes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshKey, setRefreshKey] = useState(0);   // ← 新增：用嚟觸發重新 fetch

  useEffect(() => {
    const fetchNotes = async () => {
      try {
        const response = await axios.get('http://localhost:5027/api/Notes', {
          headers: {
            Authorization: `Bearer ${token}`
          },
          params: {
            pageNumber: 1,
            pageSize: 10
          }
        });
        setNotes(response.data.items || []);
      } catch (err) {
        console.error('Fetch notes error:', err);
        alert('拎唔到 Notes，請確認 Token 有效');
      } finally {
        setLoading(false);
      }
    };
    fetchNotes();
  }, [token, refreshKey]);   // ← 新增：refreshKey 改變時重新執行

  // ← 新增：新增成功後嘅 callback
  const handleNoteAdded = () => {
    setRefreshKey(prev => prev + 1);   // 將 refreshKey +1，觸發 useEffect 重新執行
  };

  const handleDelete = async (id) => {
  if (!window.confirm('確定刪除呢個筆記？')) return;

  try {
    await axios.delete(`http://localhost:5027/api/Notes/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    setRefreshKey(prev => prev + 1);  // 重新 fetch
  } catch (err) {
    console.error('Delete error:', err);
    alert('刪除失敗');
  }
};
  if (loading) return <p>載入中...</p>;

  return (
    <div>
      {/* ← 新增：AddNote 元件 */}
      <AddNote token={token} onNoteAdded={handleNoteAdded} />

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
              <button
                onClick={() => handleDelete(note.id)}
                style={{ marginTop: 5, padding: '4px 10px', color: 'red' }}
              >
                刪除
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default NotesList;