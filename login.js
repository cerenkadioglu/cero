// Giriş yapma işlemi
async function loginUser(event) {
    event.preventDefault(); // Formun varsayılan davranışını engelle

    // Form alanlarını oku
    const username = document.getElementById('loginUsername').value;
    const password = document.getElementById('loginPassword').value;

    // API'ye veri gönder
    try {
        const response = await fetch('https://cero-api-b077343e1388.herokuapp.com/api/user/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                password: password,
            }),
        });

        if (response.ok) {
            const data = await response.json();
            alert('Giriş başarılı! Hoş geldiniz, ' + data.username);
        } else {
            const errorData = await response.json();
            alert('Giriş başarısız: ' + errorData.message);
        }
    } catch (error) {
        console.error('Hata:', error);
        alert('yapabilirsin ceren fighting !!');
    }
}

// Giriş yapma butonunu dinle
document.getElementById('loginForm').addEventListener('submit', loginUser);
