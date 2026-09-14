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
        const response = await axios.get(`${process.env.REACT_APP_API_URL}/api/Notes`, {
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
        alert('Unable to load notes. Please check that your token is valid.');
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
    if (!window.confirm('Are you sure you want to delete this note?')) return;

    try {
      await axios.delete(`${process.env.REACT_APP_API_URL}/api/Notes/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      setRefreshKey(prev => prev + 1);  // 重新 fetch
    } catch (err) {
      console.error('Delete error:', err);
      alert('Failed to delete the note.');
    }
  };

  if (loading) return <p className="text-center py-10 text-gray-500">載入中...</p>;

  return (
    <div className="max-w-3xl mx-auto">
      <AddNote token={token} onNoteAdded={handleNoteAdded} />

      <h2 className="text-2xl font-bold mb-4">我的筆記</h2>

      {notes.length === 0 ? (
        <p className="text-gray-500">暫時冇筆記</p>
      ) : (
        <ul className="space-y-4">
          {notes.map((note) => (
            <li key={note.id} className="bg-white p-5 rounded-lg shadow-sm">
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
                  <h3 className="text-lg font-semibold text-slate-800 mb-2">{note.title}</h3>
                  <p className="text-gray-600 mb-4">{note.content}</p>
                  <div className="flex gap-2">
                    <button
                      onClick={() => setEditingId(note.id)}
                      className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-1 rounded text-sm transition"
                    >
                      編輯
                    </button>
                    <button
                      onClick={() => handleDelete(note.id)}
                      className="bg-red-500 hover:bg-red-600 text-white px-4 py-1 rounded text-sm transition"
                    >
                      刪除
                    </button>
                  </div>
                </>
              )}
            </li>
          ))}
        </ul>
      )}

      <div className="flex justify-center items-center gap-4 mt-8">
        <button
          onClick={() => {
            setPageNumber(prev => Math.max(prev - 1, 1));
            setRefreshKey(prev => prev + 1);
          }}
          disabled={pageNumber === 1}
          className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded transition disabled:opacity-50"
        >
          上一頁
        </button>

        <span className="text-gray-700">第 {pageNumber} / {totalPages} 頁</span>

        <button
          onClick={() => {
            setPageNumber(prev => Math.min(prev + 1, totalPages));
            setRefreshKey(prev => prev + 1);
          }}
          disabled={pageNumber === totalPages}
          className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded transition disabled:opacity-50"
        >
          下一頁
        </button>
      </div>
    </div>
  );
}

export default NotesList;