import { useState, useEffect } from 'react';
import axios from 'axios';
import AddNote from './AddNote';   // ← 新增：引入 AddNote 元件
import EditNote from './EditNote';

function NotesList({ token }) {
  const [notes, setNotes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshKey, setRefreshKey] = useState(0);   // ← 新增：用嚟觸發重新 fetch
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [editingId, setEditingId] = useState(null);   // 記住邊個 Note 正在編輯

  useEffect(() => {
    const fetchNotes = async () => {
      try {
        const response = await axios.get('http://localhost:5027/api/Notes', {
          headers: {
            Authorization: `Bearer ${token}`
          },
          params: {
            pageNumber: pageNumber,
            pageSize: 10
          }
        });
        setNotes(response.data.items || []);
        setTotalPages(response.data.totalPages || 1);   // ← 儲存總頁數
      } catch (err) {
        console.error('Fetch notes error:', err);
        alert('拎唔到 Notes，請確認 Token 有效');
      } finally {
        setLoading(false);
      }
    };
    fetchNotes();
  }, [token, refreshKey, pageNumber]);   // ← 新增：refreshKey 改變時重新執行

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
              {editingId === note.id ? (
                <EditNote
                  token={token}
                  note={note}
                  onNoteUpdated={() => {
                    setEditingId(null);
                    setRefreshKey(prev => prev + 1);
                  }}
                  onCancel={() => setEditingId(null)}
                />
              ) : (
                <>
                  <strong>{note.title}</strong>
                  <p style={{ margin: '4px 0 0 0', color: '#666', fontSize: 14 }}>
                    {note.content}
                  </p>
                  <button
                    onClick={() => setEditingId(note.id)}
                    style={{ marginTop: 5, marginRight: 10, padding: '4px 10px' }}
                  >
                    編輯
                  </button>
                  <button
                    onClick={() => handleDelete(note.id)}
                    style={{ marginTop: 5, padding: '4px 10px', color: 'red' }}
                  >
                    刪除
                  </button>
                </>
              )}
            </li>
          ))}
        </ul>
      )}
      <div style={{ marginTop: 20, textAlign: 'center' }}>
        <button
          onClick={() => setPageNumber(prev => Math.max(prev - 1, 1))}
          disabled={pageNumber === 1}
          style={{ padding: '8px 16px', marginRight: 10 }}
        >
          上一頁
        </button>

        <span>第 {pageNumber} / {totalPages} 頁</span>

        <button
          onClick={() => setPageNumber(prev => Math.min(prev + 1, totalPages))}
          disabled={pageNumber === totalPages}
          style={{ padding: '8px 16px', marginLeft: 10 }}
        >
          下一頁
        </button>
      </div>
    </div>
  );
}

export default NotesList;