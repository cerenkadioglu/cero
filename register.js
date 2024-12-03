// Kayıt formu gönderildiğinde tetiklenecek fonksiyon
async function registerUser(event) {
    event.preventDefault(); // Formun varsayılan davranışını engelle

    // Form alanlarını oku
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    

    // API'ye veri gönder
    try {
        const response = await fetch('https://cero-api-b077343e1388.herokuapp.com/api/user/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                email: email,
                password: password,
            
            }),
        });

        if (response.ok) {
            const data = await response.json();
            alert('Kayıt başarılı! ' + data.message);
        } else {
            const errorData = await response.json();
            alert('Kayıt başarısız: ' + errorData.message);
        }
    } catch (error) {
        console.error('Hata:', error);
        alert('yeniden !');
    }
}

// Kayıt olma butonunu dinle
document.getElementById('registerForm').addEventListener('submit', registerUser);
