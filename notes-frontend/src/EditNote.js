import { useState } from 'react';
import axios from 'axios';

function EditNote({ token, note, onNoteUpdated, onCancel }) {
  const [title, setTitle] = useState(note.title);
  const [content, setContent] = useState(note.content);
  const [category, setCategory] = useState(note.category);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await axios.put(
        `${process.env.REACT_APP_API_URL}/api/Notes/${note.id}`,
        {
          title,
          content,
          category
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );

      onNoteUpdated();

    } catch (err) {
      console.error('Update note error:', err);
      setError('Failed to update the note. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

return (
  <div>
    <h3 className="text-lg font-bold mb-4">編輯筆記</h3>
    <form onSubmit={handleSubmit}>
      <div className="mb-4">
        <label className="block mb-2 font-medium">標題：</label>
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
          className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
        />
      </div>
      <div className="mb-4">
        <label className="block mb-2 font-medium">內容：</label>
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          required
          rows={3}
          className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
        />
      </div>
      <div className="mb-4">
        <label className="block mb-2 font-medium">分類：</label>
        <select
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
        >
          <option value="general">一般</option>
          <option value="dairy">日記</option>
          <option value="password">密碼</option>
        </select>
      </div>
      <div className="flex gap-2">
        <button
          type="submit"
          disabled={loading}
          className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded transition disabled:opacity-50"
        >
          {loading ? '更新中...' : '更新'}
        </button>
        <button
          type="button"
          onClick={onCancel}
          className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded transition"
        >
          取消
        </button>
      </div>
      {error && <p className="text-red-500 mt-3 text-sm">{error}</p>}
    </form>
  </div>
);
}

export default EditNote;