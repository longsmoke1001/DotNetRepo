'use client';

import { useState, useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Link from 'next/link';
import './globals.css';

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const [token, setToken] = useState<string | null>(null);
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    const savedToken = localStorage.getItem('token');
    setToken(savedToken);
  }, [pathname]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken(null);
    router.push('/login');
  };

  return (
    <html lang="zh-HK">
      <body className="min-h-screen bg-gray-100">
        {token && (
          <nav className="bg-slate-800 text-white px-6 py-4 flex justify-between items-center shadow-md">
            <Link href="/notes" className="font-semibold hover:underline">
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
          {children}
        </div>
      </body>
    </html>
  );
}