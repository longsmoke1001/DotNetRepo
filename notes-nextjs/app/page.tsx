'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';

export default function HomePage() {
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      router.push('/notes');
    } else {
      router.push('/login');
    }
  }, [router]);

  return <p className="text-center py-10">載入中...</p>;
}