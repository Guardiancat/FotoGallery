document.addEventListener('DOMContentLoaded', function () {
    // Full-screen photo modal script
    const modal = document.getElementById("photoModal");
    const modalImg = document.getElementById("fullPhoto");
    const captionText = document.getElementById("caption");

    if (modal && modalImg && captionText) {
        document.querySelectorAll('.gallery-photo').forEach(item => {
            item.addEventListener('click', event => {
                modal.style.display = "block";
                modalImg.src = event.target.src;
                captionText.innerHTML = event.target.alt;
            });
        });

        const span = document.getElementsByClassName("close")[0];
        if (span) {
            span.onclick = function () {
                modal.style.display = "none";
            };
        }
    }

    // Redirect to comments page
    document.querySelectorAll('.view-comments').forEach(button => {
        button.addEventListener('click', () => {
            const photoId = button.getAttribute('data-photo-id');
            window.location.href = `/Comments/PhotoWithComments/${photoId}`;
        });
    });

    // Обработка кастомной кнопки выбора файла
    const customFileButton = document.getElementById('customFileButton');
    const fileInput = document.getElementById('fileInput');
    const fileChosen = document.getElementById('fileChosen');
    const uploadButton = document.getElementById('uploadButton');
    const uploadForm = document.getElementById('uploadForm');

    if (customFileButton && fileInput && fileChosen) {
        customFileButton.addEventListener('click', () => {
            fileInput.click();
        });

        fileInput.addEventListener('change', () => {
            const fileName = fileInput.files.length > 0 ? fileInput.files[0].name : 'No file chosen';
            fileChosen.textContent = fileName;
        });
    }

    if (uploadButton && uploadForm) {
        uploadButton.addEventListener('click', () => {
            if (fileInput.files.length > 0) {
                uploadForm.submit();
            } else {
                alert('Please choose a file to upload.');
            }
        });
    }

    // Добавление комментариев
    const commentForm = document.getElementById('commentForm');
    if (commentForm) {
        commentForm.addEventListener('submit', async function (event) {
            event.preventDefault();

            // Установка текущей даты в скрытое поле перед отправкой формы
            const datePostedInput = document.getElementById('datePosted');
            if (datePostedInput) {
                datePostedInput.value = new Date().toISOString();
            }

            const formData = new FormData(this);

            try {
                const response = await fetch('/Comments/Create', {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'Accept': 'application/json'
                    }
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    console.error('Error:', errorData);
                    alert('Error submitting comment: ' + JSON.stringify(errorData));
                } else {
                    location.reload(); // Перезагружаем страницу для обновления комментариев
                }
            } catch (error) {
                console.error('Error submitting comment:', error);
                alert('Error submitting comment: ' + error.message);
            }
        });
    }

    // Показ формы добавления комментариев
    const addCommentButton = document.getElementById('addCommentButton');
    if (addCommentButton) {
        addCommentButton.addEventListener('click', function () {
            const commentFormContainer = document.getElementById('commentFormContainer');
            if (commentFormContainer) {
                commentFormContainer.style.display = 'block';
            }
            addCommentButton.style.display = 'none';
        });
    }
});