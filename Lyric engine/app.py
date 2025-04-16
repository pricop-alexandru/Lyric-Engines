import tkinter as tk
from tkinter import messagebox
from PIL import Image, ImageTk
import requests # asta foloseste API-ul (un mare lifesaver)
from io import BytesIO

# calitatea pe laptop de 2.5k era sub-optima asa ca am gasit libraria asta cu dll sa imi incarce high res
try:
    from ctypes import windll
    windll.shcore.SetProcessDpiAwareness(1)
except:
    pass

# asta este API-ul in sine
def search_songs_by_lyrics(lyrics):
    api_key = 'REDACTED'  # a nu se fura
    base_url = 'https://api.genius.com'
    headers = {'Authorization': 'Bearer ' + api_key}
    search_url = base_url + '/search'
    data = {'q': lyrics}
    response = requests.get(search_url, params=data, headers=headers)
    
    if response.status_code == 200:
        results = response.json()['response']['hits']
        for hit in results:
            if hit["type"] == "song":
                return hit["result"]
    else:
        messagebox.showerror("Error", "Failed to retrieve songs")
        return None

# aici arata ecranul de cautare (tranzitie intre ecrane)
def search_songs():
    clear_screen()
    search_button.pack_forget()
    entry.pack(pady=10)
    submit_button.pack(pady=10)

# functia asta face searchul prin API si returneaza ori cantec ori un pop-up
def perform_search():
    lyrics = entry.get()
    song_info = search_songs_by_lyrics(lyrics)
    if song_info:
        display_song_info(song_info)
    else:
        messagebox.showinfo("No Results", "No songs found with these lyrics")

# informatia cantecului (extragere)
def display_song_info(song_info):
    entry.pack_forget()
    submit_button.pack_forget()

    artist_name = song_info['primary_artist']['name']
    song_name = song_info['title']
    song_url = song_info['url']
    thumbnail_url = song_info['song_art_image_thumbnail_url']
    
    result_frame = tk.Frame(inner_frame, bg='#713ABE')
    result_frame.pack(pady=20, expand=True)
    
    # thumbnail-ul cantecului
    response = requests.get(thumbnail_url)
    img_data = response.content
    img = Image.open(BytesIO(img_data))
    img = img.resize((150, 150), Image.Resampling.LANCZOS)
    img_tk = ImageTk.PhotoImage(img)
    
    thumbnail_label = tk.Label(result_frame, image=img_tk, bg='#713ABE')
    thumbnail_label.image = img_tk  # Keep a reference to avoid garbage collection
    thumbnail_label.grid(row=0, column=0, rowspan=3, padx=10)
    
    # informatia cantecului (aspect)
    artist_label = tk.Label(result_frame, text=f"Artist: {artist_name}", bg='#713ABE', fg="white", font=('Open Sans', 16, 'bold'))
    artist_label.grid(row=0, column=1, sticky='w', padx=10)
    
    song_label = tk.Label(result_frame, text=f"Song Name: {song_name}", bg='#713ABE', fg="white", font=('Open Sans', 16, 'bold'))
    song_label.grid(row=1, column=1, sticky='w', padx=10)
    
    link_label = tk.Label(result_frame, text="Go to Genius song page", bg='#713ABE', fg="white", cursor="hand2", font=('Open Sans', 16, 'bold', 'underline'))
    link_label.grid(row=2, column=1, sticky='w', padx=10)
    link_label.bind("<Button-1>", lambda e: open_link(song_url))
    
    # buton pentru revenire la search
    return_button = tk.Button(inner_frame, text="Search Again", command=reset_to_search, bg="#5B0888", fg="white", font=('Open Sans', 14, 'bold'), relief='flat')
    return_button.pack(pady=20)

# resetare cautarea
def reset_to_search():
    clear_screen()
    entry.pack(pady=10)
    submit_button.pack(pady=10)

# deschidere url in website
def open_link(url):
    import webbrowser
    webbrowser.open(url, new=1)

# curatare ecran
def clear_screen():
    for widget in inner_frame.winfo_children():
        widget.pack_forget()
        widget.grid_forget()

# ecran principal
root = tk.Tk()
root.title("Lyrics Finder")
root.geometry("1500x750")
root.configure(bg='#713ABE')  # culoare fundal

# bordura (culori si aspect)
outer_border_frame = tk.Frame(root, bg='#E5CFF7', bd=5, relief='flat')
outer_border_frame.pack(pady=0, padx=0, fill='both', expand=True)

inner_border_frame = tk.Frame(outer_border_frame, bg='#9D76C1', bd=5, relief='flat')
inner_border_frame.pack(pady=5, padx=5, fill='both', expand=True)

# fundalul aplicatiei (inca ma gandesc la paleta de culori)
inner_frame = tk.Frame(inner_border_frame, bg='#713ABE')
inner_frame.pack(pady=10, padx=10, fill='both', expand=True)

# centrare la cam tot ce contine aplicatia
center_frame = tk.Frame(inner_frame, bg='#713ABE')
center_frame.place(relx=0.5, rely=0.5, anchor='center')

# incarcare font (l-am descarcat in pc inainte)
button_font = ('Open Sans', 14, 'bold')

# buton cautare
search_button = tk.Button(center_frame, text="Search by Lyrics", command=search_songs, bg="#5B0888", fg="white", font=('Open Sans', 14, 'bold'), relief='flat')
search_button.pack(pady=10)

# widget (search bar)
entry = tk.Entry(center_frame, width=50, font=button_font, bg='#5B0888', fg='white', relief='flat', borderwidth=2, highlightthickness=2, highlightbackground='#9D76C1')

# buton submit
submit_button = tk.Button(center_frame, text="Submit", command=perform_search, bg="#5B0888", fg="white", font=('Open Sans', 14, 'bold'), relief='flat')

# loop-ul aplicatiei
root.mainloop()
